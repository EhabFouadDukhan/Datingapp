import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { tick } from '@angular/core/testing';
import { take } from 'rxjs/operators';
import { User } from '../_models/User';
import { AccountService } from '../_service/account.service';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole :string[];
  user:User;

  constructor(private viewContainerRef:ViewContainerRef, private templatRef:TemplateRef<any> ,
            private accountService:AccountService) { 
              this.accountService.CurrentUser$.pipe(take(1)).subscribe(user =>{
                this.user = user;
              })
            }
  ngOnInit(): void {
    //cler view if on roles
    if(!this.user?.roles || this.user == null){
      this.viewContainerRef.clear();
      return;
    }

    if(this.user?.roles.some(r => this.appHasRole.includes(r))){
      this.viewContainerRef.createEmbeddedView(this.templatRef);
    }else{
      this.viewContainerRef.clear();
    }
  }

}
