import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map} from 'rxjs/operators';
import { User } from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl ='https://localhost:5006/apiiii/';
  private CurrentUserSource = new ReplaySubject<User>(1);
  CurrentUser$ = this.CurrentUserSource.asObservable();
  constructor(private http: HttpClient) {
    
   }
   login(module: any){
    return this.http.post(this.baseUrl + 'account/login',module).pipe(
      map((response:User) => {
          const user = response;
          if(user){
            localStorage.setItem('user', JSON.stringify(user));
            this.CurrentUserSource.next(user)
          }
      })
    )
   
   }

   register(module: any){
    return this.http.post(this.baseUrl + 'account/register',module).pipe(
      map((user:User) =>{
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.CurrentUserSource.next(user);
        }
      })
    )

   }

   setCurrentUser(user: User){
    this.CurrentUserSource.next(user);
   } 
   logout(){
    localStorage.removeItem('user');
    this.CurrentUserSource.next(null);
   }
   
}
