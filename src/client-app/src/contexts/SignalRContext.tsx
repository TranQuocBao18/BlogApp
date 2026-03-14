/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import * as signalR from '@microsoft/signalr';
import {
  createContext,
  useContext,
  useEffect,
  useState,
  type ReactNode,
} from 'react';

/**
 * Constants
 */
import { APP_CONFIG, ENV_CONFIG } from '@/constants/global';

/**
 * Types
 */
import type { HubConnection } from '@microsoft/signalr';

interface SignalRContextType {
  isConnected: boolean;
  isConnecting: boolean;
  error: string | null;
  connect: () => Promise<void>;
  disconnect: () => Promise<void>;
  send: (methodName: string, ...args: unknown[]) => Promise<void>;
  on: (eventName: string, callback: (...args: unknown[]) => void) => void;
  off: (eventName: string) => void;
  connection: HubConnection | null;
}

const SignalRContext = createContext<SignalRContextType | undefined>(undefined);

export const useSignalRContext = () => {
  const context = useContext(SignalRContext);
  if (!context) {
    throw new Error('useSignalRContext must be used within SignalRProvider');
  }
  return context;
};

interface SignalRProviderProps {
  children: ReactNode;
  autoConnect?: boolean;
}

export const SignalRProvider = ({
  children,
  autoConnect = true,
}: SignalRProviderProps) => {
  const [isConnected, setIsConnected] = useState(false);
  const [isConnecting, setIsConnecting] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [connection, setConnection] = useState<HubConnection | null>(null);

  const connect = async () => {
    // Nếu đã kết nối, không kết nối lại
    if (
      connection &&
      connection.state === signalR.HubConnectionState.Connected
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
      let conn = connection;
      if (!conn) {
        conn = new signalR.HubConnectionBuilder()
          .withUrl(`${ENV_CONFIG.API.replace('/api', '')}/hubs`, {
            accessTokenFactory: () => token,
          })
          .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
          .withServerTimeout(30000)
          .build();

        // Lắng nghe sự kiện "ReceiveMessage"
        conn.on('ReceiveMessage', (data) => {
          console.log('📨 ReceiveMessage:', data);
        });

        // Lắng nghe sự kiện "NOTIFICATION"
        conn.on('NOTIFICATION', (data) => {
          console.log('🔔 NOTIFICATION:', data);
        });

        // Lắng nghe sự kiện disconnect
        conn.onclose(() => {
          console.log('❌ SignalR disconnected');
          setIsConnected(false);
        });

        conn.onreconnected(() => {
          console.log('✅ SignalR reconnected');
          setIsConnected(true);
        });

        setConnection(conn);
      }

      await conn.start();
      console.log('✅ SignalR connected successfully');
      setIsConnected(true);
      setIsConnecting(false);
    } catch (err) {
      const message = err instanceof Error ? err.message : String(err);
      console.error('❌ SignalR connection error:', message);
      setError(message);
      setIsConnecting(false);
    }
  };

  const disconnect = async () => {
    if (connection) {
      try {
        await connection.stop();
        console.log('❌ SignalR disconnected');
        setIsConnected(false);
      } catch (err) {
        console.error('Error disconnecting:', err);
      }
    }
  };

  const send = async (methodName: string, ...args: unknown[]) => {
    if (!connection || !isConnected) {
      throw new Error('SignalR not connected');
    }
    try {
      await connection.send(methodName, ...args);
    } catch (err) {
      console.error(`Error sending ${methodName}:`, err);
      throw err;
    }
  };

  const on = (eventName: string, callback: (...args: unknown[]) => void) => {
    if (connection) {
      connection.on(eventName, callback);
    }
  };

  const off = (eventName: string) => {
    if (connection) {
      connection.off(eventName);
    }
  };

  /**
   * Auto connect khi có token trong localStorage
   */
  useEffect(() => {
    if (autoConnect) {
      const token = localStorage.getItem(APP_CONFIG.ACCESS_TOKEN);
      if (token && !isConnected && !isConnecting && !connection) {
        connect();
      }
    }
  }, [autoConnect, isConnected, isConnecting, connection, connect]);

  const value: SignalRContextType = {
    isConnected,
    isConnecting,
    error,
    connect,
    disconnect,
    send,
    on,
    off,
    connection,
  };

  return (
    <SignalRContext.Provider value={value}>{children}</SignalRContext.Provider>
  );
};
