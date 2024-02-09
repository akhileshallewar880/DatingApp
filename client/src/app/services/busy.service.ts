import { Injectable } from '@angular/core';
import { NgxSpinner, NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  busyCounter = 0;

  constructor(private spinnerService : NgxSpinnerService) { }

  busy(){
    this.busyCounter++;
    this.spinnerService.show();
  }

  idle(){
    this.busyCounter--;
    if(this.busyCounter <= 0){
      this.busyCounter = 0;
      this.spinnerService.hide();
    }
  }
}
