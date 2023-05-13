import { Injectable } from '@angular/core';
import { MsalService } from '@azure/msal-angular';

const config = {
  auth: {
    clientId: 'c6e2c81b-6c8b-4c4f-b914-4049a7326499',
    authority: 'https://login.microsoftonline.com/YOUR-TENANT-ID'
  }
};


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private msalService: MsalService) {
 
  }
  getToken(): void {
    this.msalService.acquireTokenSilent({
      scopes: ['https://graph.microsoft.com/user.read']
    }).subscribe(res => {
      // Token acquired successfully
      const accessToken = res.accessToken;
      // Add the token to your API requests
    });
    
   
  }
}
