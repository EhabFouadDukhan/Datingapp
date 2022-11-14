import { Message } from 'src/app/_models/message';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Member } from 'src/app/_models/Member';
import { MembersService } from 'src/app/_service/members.service';
import { MessageService } from 'src/app/_service/message.service';
// import {NgxGalleryOptions} from '@kolkov/ngx-gallery';
// import {NgxGalleryImage} from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-members-detail',
  templateUrl: './members-detail.component.html',
  styleUrls: ['./members-detail.component.css']
})
export class MembersDetailComponent implements OnInit {
  @ViewChild('memberTabs', {static: true}) memberTabs: TabsetComponent;
  member:Member;
  messages: Message[] = [];
  activeTab:TabDirective;
  // galleryOptions: NgxGalleryOptions[];
  // galleryImages: NgxGalleryImage[];

  constructor(private memberService:MembersService,private route:ActivatedRoute,
        private messageService:MessageService) { }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.member = data.member;
    })

    this.route.queryParams.subscribe(params =>{
      params.tab ? this.selectTab(params.tab) : this.selectTab(0);
    })

    // this.galleryOptions =[
    //   {
    //   width:'500px',
    //   height:'500px',
    //   imagePercent: 100,
    //   thumbnailsColumns: 4,
    //   imageAnimation: NgxGalleryAnimation.Slide,
    //   preview: false
    //   }
    // ]

    // this.galleryImages = this.getImages();
  }
  // getImages(): NgxGalleryImage[]{
  //   const imageUrls = []
  //   for(const photo of this.member.photos){
  //     imageUrls.push({
  //       small: photo?.url,
  //       medium: photo?.url,
  //       big: photo?.url
  //     })
  //   }
  //   return imageUrls;
  // }

  

  loadMessages() {
    this.messageService.getMessageThread(this.member.username).subscribe(messages => {
      this.messages = messages;
    })
  }
  onTabActivated(data: TabDirective){
    this.activeTab = data;
    if(this.activeTab.heading ==='Messages'&& this.messages.length === 0){
        this.loadMessages();
    }
  }

  selectTab(tabId : number){
    this.memberTabs.tabs[tabId].active = true;
  }


  // loadMember(){
  //   this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscribe(member =>{
  //     this.member = member;
      
  //   })
  // }

}
