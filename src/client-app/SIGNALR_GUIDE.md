# SignalR Integration Guide

## 📋 Giới thiệu

SignalR đã được tích hợp vào ứng dụng React. Khi bạn đăng nhập, kết nối SignalR sẽ **tự động được thiết lập** và duy trì trong suốt phiên làm việc.

## 🔧 Cách hoạt động

### 1. **Auto Connection**
- Sau khi bạn đăng nhập thành công, token được lưu vào `localStorage`
- `SignalRProvider` tự động phát hiện token và kết nối đến SignalR hub
- Kết nối sẽ được duy trì tự động khi ngắt (auto-reconnect)

### 2. **Cấu trúc**
```
src/
├── hooks/
│   ├── useSignalR.ts              # Hook cơ bản (nếu không dùng context)
│   └── useSignalRListener.ts      # Helper hooks (nghe events, gửi messages)
├── contexts/
│   └── SignalRContext.tsx         # Context provider cho SignalR
└── main.tsx                       # Đã wrap với SignalRProvider
```

## 📖 Cách sử dụng

### 1. **Nghe sự kiện từ Server** (ReceiveMessage hoặc NOTIFICATION)

```typescript
import { useSignalRListener } from '@/hooks/useSignalRListener';

function DashboardPage() {
  useSignalRListener({
    eventName: 'ReceiveMessage',  // hoặc 'NOTIFICATION'
    onMessage: (data) => {
      console.log('📨 Nhận thông báo:', data);
      // Xử lý logic thông báo
    },
    enabled: true,  // Optional: control khi nào kích hoạt
  });

  return <div>Dashboard</div>;
}
```

### 2. **Gửi message tới Server**

```typescript
import { useSignalRSend } from '@/hooks/useSignalRListener';

function SendMessageComponent() {
  const { sendMessage, isConnected } = useSignalRSend({
    timeout: 5000, // Optional: timeout cho request
  });

  const handleSendNotification = async () => {
    try {
      await sendMessage('SendNotification', 'Hello Server');
      console.log('✅ Gửi thành công');
    } catch (error) {
      console.error('❌ Gửi thất bại:', error);
    }
  };

  return (
    <button disabled={!isConnected} onClick={handleSendNotification}>
      Gửi Thông báo
    </button>
  );
}
```

### 3. **Theo dõi trạng thái kết nối**

```typescript
import { useSignalRStatus } from '@/hooks/useSignalRListener';

function ConnectionStatus() {
  const { isConnected, isConnecting, error } = useSignalRStatus();

  return (
    <div>
      {isConnecting && <p>⏳ Đang kết nối...</p>}
      {isConnected && <p>✅ Đã kết nối</p>}
      {error && <p>❌ Lỗi: {error}</p>}
    </div>
  );
}
```

### 4. **Sử dụng Context trực tiếp** (Advanced)

```typescript
import { useSignalRContext } from '@/contexts/SignalRContext';

function AdvancedComponent() {
  const { isConnected, send, on, off, disconnect } = useSignalRContext();

  // Đăng ký nghe event
  useEffect(() => {
    if (isConnected) {
      on('NOTIFICATION', (data) => {
        console.log('🔔 Notification:', data);
      });

      return () => off('NOTIFICATION');
    }
  }, [isConnected, on, off]);

  // Gửi message
  const handleConnect = async () => {
    try {
      await send('SendMessage', { text: 'Hello' });
    } catch (error) {
      console.error('Error:', error);
    }
  };

  // Ngắt kết nối (nếu cần)
  const handleDisconnect = async () => {
    await disconnect();
  };

  return (
    <>
      <button onClick={handleConnect}>Gửi</button>
      <button onClick={handleDisconnect}>Ngắt kết nối</button>
    </>
  );
}
```

## ⚙️ Cấu hình

### Backend Hub URL
- Mặc định: `http://localhost:5151/api/hubs`
- Được cấu hình từ `ENV_CONFIG.API` trong `src/constants/global/index.ts`

### Auto Reconnect
```typescript
.withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
```
- Sẽ tự động kết nối lại với các độ trễ: 0ms, 2s, 5s, 10s, 30s

### Server Timeout
```typescript
.withServerTimeout(30000)
```
- Timeout sau 30 giây nếu không nhận được phản hồi

## 🔐 Token Management
- Token được lưu tự động khi login thành công
- SignalR sử dụng token này để xác thực với server
- Khi logout, token bị xóa và kết nối sẽ bị ngắt

## 📝 Ví dụ Full Component

```typescript
import { useEffect } from 'react';
import { useSignalRListener, useSignalRStatus } from '@/hooks/useSignalRListener';

interface Notification {
  title: string;
  message: string;
  createdAt: string;
}

export function NotificationCenter() {
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const { isConnected, isConnecting, error } = useSignalRStatus();

  useSignalRListener({
    eventName: 'ReceiveMessage',
    onMessage: (data) => {
      setNotifications((prev) => [data as Notification, ...prev]);
    },
  });

  return (
    <div>
      {isConnecting && <p>Loading notifications...</p>}
      {error && <p className="text-red-500">Error: {error}</p>}
      {isConnected && (
        <>
          <p className="text-green-500">✅ Connected</p>
          <ul>
            {notifications.map((n) => (
              <li key={n.createdAt}>
                <strong>{n.title}</strong>: {n.message}
              </li>
            ))}
          </ul>
        </>
      )}
    </div>
  );
}
```

## 🐛 Troubleshooting

### Không kết nối được
- Kiểm tra token có được lưu vào `localStorage` không
- Kiểm tra backend SignalR hub URL có đúng không
- Mở DevTools -> Console để xem log kết nối

### Kết nối mà không nhận được message
- Đảm bảo backend gửi event đúng tên ("ReceiveMessage" hoặc "NOTIFICATION")
- Kiểm tra user có quyền nhận thông báo không
- Kiểm tra backend có join client vào group/user đúng không

### Kết nối bị ngắt liên tục
- Kiểm tra network connection
- Kiểm tra backend có lỗi gì không
- Kiểm tra CORS policy (backend-side)
- Xem DevTools -> Network -> WS để theo dõi WebSocket

## 🚀 Next Steps

1. **Backend**: Đảm bảo SignalR hub được setup đúng
2. **Emit Events**: Backend gửi thông báo bằng cách:
   ```csharp
   await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", notification);
   ```
3. **Handle Events**: Frontend nghe và xử lý events
4. **UI Update**: Update UI khi nhận được notification

---

**Note**: Hiện tại SignalR provider sẽ tự động kết nối khi phát hiện token trong localStorage. Không cần manual call `connect()` trong hầu hết trường hợp.
