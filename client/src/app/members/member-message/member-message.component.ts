import { Message } from 'src/app/_models/message';
import { ChangeDetectionStrategy, Component, Input, OnInit, ViewChild } from '@angular/core';
import { MessageService } from 'src/app/_service/message.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-message',
  templateUrl: './member-message.component.html',
  styleUrls: ['./member-message.component.css']
})
export class MemberMessageComponent implements OnInit {
@ViewChild('messageForm') messageForm: NgForm;  
@Input() username : string;
@Input() messages: Message[];
messageContint: string;
  constructor(private messageService:MessageService) { }

  ngOnInit(): void {
    
  }

  sendMessage(){
    this.messageService.sendMessage(this.username,this.messageContint).subscribe(messages =>{
      this.messages.push(messages);
      this.messageForm.reset();
    })
  }

  

}
