import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any){ 
    return this.http.post<User>(this.baseUrl + "account/login", model).pipe(
      map(user => this.setData(user))
    )
  }

  register(model: any){ 
    return this.http.post<User>(this.baseUrl + "account/register", model).pipe(
      map(user => this.setData(user))
    )
  }
  
  setData(user: User){
    if (user){
      localStorage.setItem("user", JSON.stringify(user));
      this.currentUserSource.next(user);
    }
  }

  setCurrentUser(user: User){
    this.currentUserSource.next(user);
  }

  logout(){ 
    localStorage.removeItem("user");
    this.currentUserSource.next(null); 
  }

}