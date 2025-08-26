import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class SignupService {
  private baseUrl = environment.apiUrl;
  constructor(private readonly http: HttpClient) { }

  createaccount(accountdetails: { name: string, username: string, email: string, password: string, createdAt: Date  }): Observable<any> {
    return this.http.post(this.baseUrl + '/api/User/CreateUser', accountdetails);
  }
}
