import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountserviceService } from '../services/accountservice.service';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountserviceService);
  const toaster = inject(ToastrService);

  return accountService.currentUser$.pipe(
    (map(user => {
      if(user) {
        return true;
      } else {
        toaster.warning('Action Not Allowed !');
        return false;
      }
    }))
  )
};
