import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/User';
import { AccountService } from '../_service/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  module: any = {}
  CurrentUser$: Observable<User>;

  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
    
  }
  Login(){
    
    this.accountService.login(this.module).subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    })
  }
  Logout(){
    this.accountService.logout();
  }
  

}
