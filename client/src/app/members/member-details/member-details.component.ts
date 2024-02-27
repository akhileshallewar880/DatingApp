import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryModule, ImageItem } from 'ng-gallery';
import { TabDirective, TabsModule, TabsetComponent } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';
import { Member } from 'src/app/models/member';
import { MembersService } from 'src/app/services/members.service';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import { MessageService } from 'src/app/services/message.service';
import { Message } from 'src/app/models/message';

@Component({
  selector: 'app-member-details',
  standalone : true,
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css'],
  imports: [CommonModule, TabsModule, GalleryModule, TimeagoModule, MemberMessagesComponent]
})
export class MemberDetailsComponent implements OnInit{
  @ViewChild('memberTabs', {static : true}) memberTabs? : TabsetComponent
  member : Member = {} as Member;
  images : GalleryModule[] = [];
  activeTab? : TabDirective;
  messages: Message[] = [];

  constructor(private memberService : MembersService, private route : ActivatedRoute, private messageService : MessageService) {}

  ngOnInit(): void {
      this.route.data.subscribe({
        next : data => this.member = data['member']
      })

      this.route.queryParams.subscribe({
        next: params => {
          params['tab'] && this.selectTab(params['tab'])
        }
      })

      this.getImages();
  }


  selectTab(heading : string) {
    if(this.memberTabs) {
      this.memberTabs.tabs.find(x => x.heading === heading)!.active = true;
    }
  }

  OnTabActivated(data : TabDirective) {
    this.activeTab = data;
    if(this.activeTab.heading === 'Messages') {
      this.loadMessages();
    }
  }

  loadMessages() {
    if (this.member) {
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next : messages => this.messages = messages
      })
    }
  }

  getMemberDetail(){
    const username = this.route.snapshot.paramMap.get('username');
    if(!username) return;

    this.memberService.getMember(username).subscribe({
      next : resp => {
        this.member = resp,
        this.getImages()
      },
      error : err => console.log(err)
      
    })
  }

  getImages(){
    if(!this.member) return;

    for (const photo of this.member?.photos){
      this.images.push(new ImageItem({src : photo.url, thumb : photo.url}))
    }
  }

}
