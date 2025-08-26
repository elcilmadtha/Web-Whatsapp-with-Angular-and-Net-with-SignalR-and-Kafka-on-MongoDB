export interface ChatMessage {
  sender: string;
  receiver: string;
  content: string;
  timestamp: Date;
}
export interface User {
  id: string;
  name: string;
}
