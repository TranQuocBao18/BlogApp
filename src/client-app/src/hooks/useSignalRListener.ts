/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { useSignalRContext } from '@/contexts/SignalRContext';
import { useCallback, useEffect } from 'react';

interface UseSignalRListenerOptions {
  eventName: string;
  onMessage?: (data: unknown) => void;
  enabled?: boolean;
}

/**
 * Hook để lắng nghe sự kiện từ SignalR
 */
export const useSignalRListener = (options: UseSignalRListenerOptions) => {
  const { eventName, onMessage, enabled = true } = options;
  const { on, off, isConnected } = useSignalRContext();

  useEffect(() => {
    if (enabled && isConnected && onMessage) {
      on(eventName, onMessage);

      return () => {
        off(eventName);
      };
    }
  }, [enabled, isConnected, eventName, onMessage, on, off]);
};

interface UseSignalRSendOptions {
  timeout?: number;
}

/**
 * Hook để gửi message qua SignalR
 */
export const useSignalRSend = (options: UseSignalRSendOptions = {}) => {
  const { timeout = 5000 } = options;
  const { send, isConnected } = useSignalRContext();

  const sendMessage = useCallback(
    async (methodName: string, ...args: unknown[]) => {
      if (!isConnected) {
        throw new Error('SignalR not connected');
      }

      return Promise.race([
        send(methodName, ...args),
        new Promise((_, reject) =>
          setTimeout(() => reject(new Error('Send timeout')), timeout),
        ),
      ]);
    },
    [send, isConnected, timeout],
  );

  return { sendMessage, isConnected };
};

/**
 * Hook để theo dõi trạng thái kết nối SignalR
 */
export const useSignalRStatus = () => {
  const { isConnected, isConnecting, error } = useSignalRContext();

  return { isConnected, isConnecting, error };
};
