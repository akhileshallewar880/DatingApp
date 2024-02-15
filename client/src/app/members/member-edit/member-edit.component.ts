import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { Form, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/models/member';
import { User } from 'src/app/models/user';
import { AccountserviceService } from 'src/app/services/accountservice.service';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  @ViewChild('editForm') editForm : NgForm | undefined;
  
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if(this.editForm?.dirty){
      $event.returnValue = true;
    }
  }

  member : Member | undefined;

  user : User | null = null;
  

  constructor(private accountService : AccountserviceService, private memberService : MembersService, private toastr : ToastrService) {
      accountService.currentUser$.pipe(take(1)).subscribe({
        next: resp => this.user = resp
      })
  }

  ngOnInit(): void {
      this.getMember();
  }

  getMember(){

    if(!this.user) return;
    this.memberService.getMember(this.user?.username).subscribe({
      next : resp => this.member = resp
    })

  }

  updateMember(){
    if(!this.member) return;
    this.memberService.updateMember(this.member).subscribe({
      next: _ => {
        this.toastr.success("Data Updated Successfully");
        this.editForm?.reset(this.member);
      }
    })

  }

}
