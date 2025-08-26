import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { ChatMessage } from './chat-window.model';
import { environment } from '../../environments/environment';
import { Observable, Subject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { HttpTransportType, HubConnectionState, LogLevel } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class ChatWindowService {
  private baseUrl = environment.apiUrl;
  private hubConnection?: signalR.HubConnection;
  public messages: ChatMessage[] = [];
  private messageSubject = new Subject<ChatMessage>();
  private messageQueue: ChatMessage[] = [];
  private startPromise?: Promise<void>;
  private handlersWired = false;
  messages$ = this.messageSubject.asObservable();
  constructor(private http: HttpClient) { }


  /** Start SignalR connection with automatic reconnect */
  public async startConnection(senderUserId: string): Promise<void> {

    if (this.hubConnection && this.hubConnection.state === signalR.HubConnectionState.Connected) {
      return;
    }

    if (this.startPromise) return this.startPromise;

    if (!this.hubConnection) {
      const hubUrl = `${this.baseUrl}/chatHub?userId=${encodeURIComponent(senderUserId)}`;
      this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(hubUrl, {
          accessTokenFactory: () => localStorage.getItem('authToken') || '',
          // If your server supports WebSockets directly, this helps avoid negotiate/fallback flakiness.
          transport: HttpTransportType.WebSockets,
          skipNegotiation: true, // ❗ remove if you use Azure SignalR or need negotiate
        })
        .withAutomaticReconnect([800, 1500, 2500]) // no immediate 0ms retry
        .configureLogging(LogLevel.Information)
        .build();

      this.wireHandlers(); // attach only once
    }


    this.startPromise = this.hubConnection.start()
      .then(() => {
        console.log('SignalR started', this.hubConnection?.connectionId);
      })
      .catch(err => {
        console.error('SignalR start error:', err);
        throw err;
      })
      .finally(() => {
        // allow future starts if this one failed or completed
        this.startPromise = undefined;
      });

    return this.startPromise;
  }


  private wireHandlers(): void {
    if (this.handlersWired || !this.hubConnection) return;
    this.handlersWired = true;

    this.hubConnection.onreconnecting(err => console.warn('SignalR reconnecting...', err));
    this.hubConnection.onreconnected(id => console.log('SignalR reconnected with ID:', id));

    // ✅ Ensure we don't double-register
    this.hubConnection.off('ReceiveMessage');
    this.hubConnection.on('ReceiveMessage', (message: ChatMessage) => {
      this.messageSubject.next(message);
    });
  }

  async stopConnection(): Promise<void> {
    if (this.hubConnection &&
      (this.hubConnection.state === HubConnectionState.Connected ||
        this.hubConnection.state === HubConnectionState.Connecting)) {
      await this.hubConnection.stop();
    }
    this.hubConnection = undefined;
    this.handlersWired = false;
  }

  getMessages() {
    return this.messages$;
  }

  public sendMessage(message: ChatMessage): void {
    if (this.hubConnection && this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.invoke('SendMessage', message)
        .catch(err => console.error('Send failed:', err));
    } else {
      console.warn('Connection not ready, queuing message');
      this.messageQueue.push(message);
    }
  }

  public async resetConnection(): Promise<void> {
    if (this.hubConnection) {
      try {
        await this.hubConnection.stop();
        console.log('SignalR connection stopped');
      } catch (err) {
        console.error('Error stopping SignalR:', err);
      } finally {
        this.hubConnection = undefined;
        this.startPromise = undefined;
        this.handlersWired = false; // important for re-attaching handlers
      }
    }
  }

  getChatHistory(senderId: string, receiverId: string): Observable<ChatMessage[]> {
    return this.http.get<ChatMessage[]>(`${this.baseUrl}/api/Chat/GetHistoryMessages/${senderId}/${receiverId}`);
  }
}
