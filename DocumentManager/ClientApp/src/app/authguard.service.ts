import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { MsalService } from '@azure/msal-angular';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private msalService: MsalService, private router: Router) { }

  canActivate(): boolean {
    const isAuthenticated = this.msalService.instance.getAllAccounts().length > 0;
    if (!isAuthenticated) {
      this.msalService.loginRedirect();
      return false;
    }
    return true;
  }
}
