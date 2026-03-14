/**
 * @copyright 2025 TranBao
 * @license Apache-2.0
 */

/**
 * Node modules
 */
import { useState } from 'react';

/**
 * Components
 */
import { Button } from '@/components/ui/button';

/**
 * Hooks
 */
import {
  useSignalRListener,
  useSignalRSend,
  useSignalRStatus,
} from '@/hooks/useSignalRListener';

export const SignalRDemo = () => {
  const [messages, setMessages] = useState<unknown[]>([]);
  const { isConnected, isConnecting, error } = useSignalRStatus();
  const { sendMessage } = useSignalRSend();

  /**
   * Nghe sự kiện "NOTIFICATION" từ server
   */
  useSignalRListener({
    eventName: 'NOTIFICATION',
    onMessage: (data) => {
      console.log('🔔 Received notification:', data);
      setMessages((prev) => [
        { type: 'notification', data, timestamp: new Date() },
        ...prev,
      ]);
    },
  });

  /**
   * Nghe sự kiện "ReceiveMessage" từ server
   */
  useSignalRListener({
    eventName: 'ReceiveMessage',
    onMessage: (data) => {
      console.log('📨 Received message:', data);
      setMessages((prev) => [
        { type: 'message', data, timestamp: new Date() },
        ...prev,
      ]);
    },
  });

  /**
   * Gửi test message
   */
  const handleSendMessage = async () => {
    try {
      await sendMessage('SendMessage', 'Hello from React!');
      console.log('✅ Message sent');
    } catch (error) {
      console.error('❌ Error sending message:', error);
    }
  };

  return (
    <div className='p-6 bg-gray-50 rounded-lg shadow-md'>
      <h2 className='text-xl font-bold mb-4'>SignalR Connection Status</h2>

      {/* Status Section */}
      <div className='mb-4 p-4 bg-white rounded border'>
        {isConnecting && (
          <p className='text-yellow-600 font-semibold'>⏳ Đang kết nối...</p>
        )}
        {isConnected && (
          <p className='text-green-600 font-semibold'>✅ Đã kết nối SignalR</p>
        )}
        {error && <p className='text-red-600 font-semibold'>❌ Lỗi: {error}</p>}
        {!isConnected && !isConnecting && !error && (
          <p className='text-gray-600 font-semibold'>⚪ Chưa kết nối</p>
        )}
      </div>

      {/* Action Buttons */}
      <div className='mb-4 flex gap-2'>
        <Button
          onClick={handleSendMessage}
          disabled={!isConnected}
          className='bg-blue-600 hover:bg-blue-700'
        >
          Gửi Test Message
        </Button>
      </div>

      {/* Messages Section */}
      <div className='bg-white p-4 rounded border'>
        <h3 className='font-semibold mb-2'>
          Messages Received ({messages.length}):
        </h3>
        <div className='max-h-96 overflow-y-auto'>
          {messages.length === 0 ? (
            <p className='text-gray-500 italic'>Chưa nhận message nào...</p>
          ) : (
            <ul className='space-y-2'>
              {messages.map((msg, idx) => (
                <li
                  key={idx}
                  className='p-2 bg-gray-100 rounded text-sm'
                >
                  <div className='font-semibold'>
                    {(msg as any).type === 'notification'
                      ? '🔔 NOTIFICATION'
                      : '📨 MESSAGE'}
                  </div>
                  <pre className='text-xs mt-1 overflow-auto'>
                    {JSON.stringify((msg as any).data, null, 2)}
                  </pre>
                </li>
              ))}
            </ul>
          )}
        </div>
      </div>
    </div>
  );
};
