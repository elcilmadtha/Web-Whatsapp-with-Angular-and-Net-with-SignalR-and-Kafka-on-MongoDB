import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatLayoutService } from './chat-layout.service';
import { UserListModule } from '../user-list/user-list.module';
import { ChatWindowModule } from '../chat-window/chat-window.module';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { ChatLayoutComponent } from './chat-layout.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { FormsModule } from '@angular/forms';
import { CapitalizePipe } from '../services/pipes/capitalize.pipe';

@NgModule({
  declarations: [ChatLayoutComponent, CapitalizePipe],
  imports: [
      CommonModule,
      UserListModule,
      ChatWindowModule,
      MatSidenavModule,
      MatIconModule,
      MatToolbarModule,
      MatInputModule,
      FormsModule,
      MatTooltipModule
    ],
  exports: [ChatLayoutComponent, CapitalizePipe],
    providers: [ChatLayoutService]
})
export class ChatLayoutModule { }
