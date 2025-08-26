import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatWindowComponent } from './chat-window.component';
import { ChatWindowService } from './chat-window.service';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';



@NgModule({
    declarations: [ChatWindowComponent],
  imports: [
    CommonModule,
    MatInputModule,
    FormsModule,
    MatIconModule
    ],
    exports: [ChatWindowComponent],
    providers: [ChatWindowService]
})
export class ChatWindowModule { }
