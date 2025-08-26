import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserListComponent } from './user-list.component';
import { UserListService } from './user-list.service';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
    declarations: [UserListComponent],
  imports: [
    CommonModule,
    MatInputModule,
    FormsModule,
    MatListModule,
    MatIconModule,
    HttpClientModule
    ],
    exports: [UserListComponent],
    providers: [UserListService]
})
export class UserListModule { }
