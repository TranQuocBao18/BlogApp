/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import * as signalR from '@microsoft/signalr';
import { useCallback, useEffect, useRef, useState } from 'react';

/**
 * Constants
 */
import { APP_CONFIG, ENV_CONFIG } from '@/constants/global';

export interface UseSignalROptions {
  enabled?: boolean;
  autoConnect?: boolean;
}

export const useSignalR = (options: UseSignalROptions = {}) => {
  const { enabled = true, autoConnect = true } = options;
  const connectionRef = useRef<signalR.HubConnection | null>(null);
  const [isConnected, setIsConnected] = useState(false);
  const [isConnecting, setIsConnecting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  /**
   * Khởi tạo và bắt đầu kết nối
   */
  const connect = useCallback(async () => {
    if (!enabled) return;

    // Nếu đã kết nối, không kết nối lại
    if (
      connectionRef.current &&
      connectionRef.current.state === signalR.HubConnectionState.Connected
    ) {
      return;
    }

    try {
      setIsConnecting(true);
      setError(null);

      const token = localStorage.getItem(APP_CONFIG.ACCESS_TOKEN);
      if (!token) {
        throw new Error('No access token found');
      }

      // Nếu connection đã tồn tại, dùng lại; không thì tạo mới
      if (!connectionRef.current) {
        connectionRef.current = new signalR.HubConnectionBuilder()
          .withUrl(`${ENV_CONFIG.API.replace('/api', '')}/hubs`, {
            accessTokenFactory: () => token,
          })
          .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
          .withServerTimeout(30000)
          .build();

        // Lắng nghe sự kiện "ReceiveMessage"
        connectionRef.current.on('ReceiveMessage', (data) => {
          console.log('📨 ReceiveMessage:', data);
        });

        // Lắng nghe sự kiện "NOTIFICATION"
        connectionRef.current.on('NOTIFICATION', (data) => {
          console.log('🔔 NOTIFICATION:', data);
        });

        // Lắng nghe sự kiện disconnect
        connectionRef.current.onclose(() => {
          console.log('❌ SignalR disconnected');
          setIsConnected(false);
        });

        connectionRef.current.onreconnected(() => {
          console.log('✅ SignalR reconnected');
          setIsConnected(true);
        });
      }

      await connectionRef.current.start();
      console.log('✅ SignalR connected successfully');
      setIsConnected(true);
      setIsConnecting(false);
    } catch (err) {
      const message = err instanceof Error ? err.message : String(err);
      console.error('❌ SignalR connection error:', message);
      setError(message);
      setIsConnecting(false);
    }
  }, [enabled]);

  /**
   * Ngắt kết nối
   */
  const disconnect = useCallback(async () => {
    if (connectionRef.current) {
      try {
        await connectionRef.current.stop();
        console.log('❌ SignalR disconnected');
        setIsConnected(false);
      } catch (err) {
        console.error('Error disconnecting:', err);
      }
    }
  }, []);

  /**
   * Gửi message tới server
   */
  const send = useCallback(
    async (methodName: string, ...args: unknown[]) => {
      if (!connectionRef.current || !isConnected) {
        throw new Error('SignalR not connected');
      }
      try {
        await connectionRef.current.send(methodName, ...args);
      } catch (err) {
        console.error(`Error sending ${methodName}:`, err);
        throw err;
      }
    },
    [isConnected],
  );

  /**
   * Đăng ký listener cho sự kiện
   */
  const on = useCallback(
    (eventName: string, callback: (...args: unknown[]) => void) => {
      if (connectionRef.current) {
        connectionRef.current.on(eventName, callback);
      }
    },
    [],
  );

  /**
   * Hủy đăng ký listener
   */
  const off = useCallback((eventName: string) => {
    if (connectionRef.current) {
      connectionRef.current.off(eventName);
    }
  }, []);

  /**
   * Auto connect khi component mount
   */
  useEffect(() => {
    if (autoConnect && enabled) {
      const token = localStorage.getItem(APP_CONFIG.ACCESS_TOKEN);
      if (token) {
        connect();
      }
    }

    return () => {
      // Optional: disconnect khi component unmount
      // disconnect();
    };
  }, [autoConnect, enabled, connect]);

  return {
    isConnected,
    isConnecting,
    error,
    connect,
    disconnect,
    send,
    on,
    off,
    connection: connectionRef.current,
  };
};
