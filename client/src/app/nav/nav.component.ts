import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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

  constructor(public accountService: AccountService,private router: Router,
    private toastr:ToastrService) { }

  ngOnInit(): void {
    
  }
  Login(){
    
    this.accountService.login(this.module).subscribe(response => {
      this.router.navigateByUrl('/members');
    }, error => {
      console.log(error);
      this.toastr.error(error.error);
    })
  }
  Logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
  

}
