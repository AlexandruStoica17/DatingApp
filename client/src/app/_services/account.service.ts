import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})

//angular service: de retinut: singleton, created when the application starts(ex: user refreshes their browser),
//  good for storing state and making httprequests
export class AccountService {


  private http = inject(HttpClient);  
  baseUrl= environment.apiUrl;
  currentUser = signal<User | null>(null) //initial value is set to null

  login(model: any){
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map(user => {
        if(user){
          localStorage.setItem('user',JSON.stringify(user));
          this.currentUser.set(user);
        }
              })
    )
  }
  
  register(model: any){
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if(user){
          localStorage.setItem('user',JSON.stringify(user));
          this.currentUser.set(user);
        }
        return user;
      })
    )
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}
