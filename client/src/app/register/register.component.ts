import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_service/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
module: any={};
// @Input() usersFromHomeComponent: any;
@Output() cancelRegister = new EventEmitter();
  constructor(private accountService:AccountService) { }

  ngOnInit(): void {
  }
  register(){
    // console.log(this.module);
    this.accountService.register(this.module).subscribe(response =>{
      console.log(response);
      this.cancel();
    },error =>{
      console.log(error);
    })
  }
  cancel(){
    this.cancelRegister.emit(false);
  }

}
