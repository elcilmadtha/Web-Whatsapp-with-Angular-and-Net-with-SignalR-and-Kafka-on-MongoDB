import { Component, ElementRef, Input, OnChanges, OnDestroy, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { ChatMessage, User } from './chat-window.model';
import { ChatWindowService } from './chat-window.service';
import { UsersDto } from '../user-list/user-list.model';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs/internal/Subscription';

@Component({
  selector: 'app-chat-window',
  templateUrl: './chat-window.component.html',
  styleUrls: ['./chat-window.component.scss']
})
export class ChatWindowComponent implements OnInit, OnChanges, OnDestroy {
  messageTobeSent: string = '';
  selectedUser: User | null = null;
  currentUserId: string = '';
  @Input() selectedUserDto!: UsersDto;
  messages: ChatMessage[] = [];
  showChatWindow: boolean = false;
  @ViewChild('bottomAnchor') bottomAnchor!: ElementRef;
  shouldScroll = false;
  private msgSub?: Subscription;
  constructor(private readonly chatWindowService: ChatWindowService, private readonly authService: AuthService, private readonly router: Router,
    private readonly snackBar: MatSnackBar) {
  }

  ngOnChanges(simpleChanges: SimpleChanges): void {
    if (simpleChanges && simpleChanges['selectedUserDto'].currentValue) {
      console.log(this.selectedUserDto);
      this.currentUserId = this.authService.getCurrentUserId();
      this.showChatWindow = true;
      const receiverId = this.selectedUserDto.id;
      this.chatWindowService.getChatHistory(this.currentUserId, receiverId)
        .subscribe(messages => {
          this.messages = messages;
          this.shouldScroll = true;
        });
    }
  }

  ngOnInit(): void  {
    this.currentUserId = this.authService.getCurrentUserId();
    if (!this.currentUserId || !this.authService.isTokenValid()) {
      this.handleInvalidToken();
      return;
    }
    this.initSignalR();
  }

  private async initSignalR() {
    await this.chatWindowService.startConnection(this.currentUserId);

    this.msgSub = this.chatWindowService.getMessages().subscribe(resultMessage => {
      if (
        (resultMessage.sender === this.selectedUserDto.id && resultMessage.receiver === this.currentUserId) ||
        (resultMessage.sender === this.currentUserId && resultMessage.receiver === this.selectedUserDto.id)
      ) {
        this.messages.push(resultMessage);
        this.shouldScroll = true;
      }
    });
  }

  private scrollToBottom(): void {
      this.bottomAnchor.nativeElement.scrollIntoView({ behavior: 'smooth', block: 'end' });
  }

  ngAfterViewChecked(): void {
    if (this.shouldScroll) {
      this.scrollToBottom();
      this.shouldScroll = false; 
    }
  }

  sendMessage() {
    if (this.messageTobeSent.trim()) {
      const message: ChatMessage = {
        sender: this.currentUserId,
        receiver: this.selectedUserDto.id,
        content: this.messageTobeSent,
        timestamp: new Date()
      };
      this.chatWindowService.sendMessage(message);
      this.messageTobeSent = '';
    }
  }


  private handleInvalidToken(): void {
    localStorage.removeItem('authToken');
    this.snackBar.open('Token expired/Invalid token', 'Close', {
      duration: 4000,
      verticalPosition: 'bottom',
      horizontalPosition: 'center'
    });
    this.router.navigate(['/login']);
  }

  ngOnDestroy(): void {
    this.msgSub?.unsubscribe();
  }
}
