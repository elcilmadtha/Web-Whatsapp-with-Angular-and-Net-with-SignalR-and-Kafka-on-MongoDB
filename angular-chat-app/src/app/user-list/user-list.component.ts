import { Component, EventEmitter, Output} from '@angular/core';
import { UserListService } from './user-list.service';
import { UsersDto } from './user-list.model';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent {
  searchResult: UsersDto[] = [];
  searchText: string = "";
  selectedUserId: string | null = null;
  currentUserId: string = '';
  @Output() selectedUser = new EventEmitter<UsersDto>();
  constructor(private readonly chatService: UserListService, private readonly authService: AuthService) {
    this.currentUserId = this.authService.getCurrentUserId();
    this.chatService.getRecentUsers(this.currentUserId).subscribe(response => {
      this.searchResult = response;
    });
  }

  onSearch() {
    this.chatService.getUsers(this.searchText).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          this.searchResult = response.users;
          console.log(this.searchText);
        } else {
          console.error(response.validationMessage || response.errorMessage);
        }
      },
      error: (err) => {
        console.error('HTTP Error', err);
      }
    });
  }

  selectUser(user: UsersDto) {
    this.selectedUserId = user.id;
    this.selectedUser.emit(user);
  }
}
