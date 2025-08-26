import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';
import { GetUsersResponse, UsersDto } from './user-list.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserListService {
  private baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getUsers(searchText: string): Observable<GetUsersResponse> {
    return this.http.get<GetUsersResponse>(this.baseUrl + '/api/User/SearchUser?username=' + searchText);
  }
  getRecentUsers(userId: string): Observable<UsersDto[]> {
    return this.http.get<UsersDto[]>(`${this.baseUrl}/api/User/GetRecentChatUsers/${userId}`);
  }
}
