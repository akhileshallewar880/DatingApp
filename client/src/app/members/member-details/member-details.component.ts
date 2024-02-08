import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryModule, ImageItem } from 'ng-gallery';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { Member } from 'src/app/models/member';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-details',
  standalone : true,
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css'],
  imports: [CommonModule, TabsModule, GalleryModule]
})
export class MemberDetailsComponent implements OnInit{

  member : Member | undefined;
  images : GalleryModule[] = [];

  constructor(private memberService : MembersService, private route : ActivatedRoute) {}

  ngOnInit(): void {
      this.getMemberDetail();
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
