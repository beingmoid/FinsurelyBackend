import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { MsalModule } from '@azure/msal-angular';
import { TokenInterceptor } from './token.interceptor';



import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { IPublicClientApplication, PublicClientApplication } from '@azure/msal-browser';
const msalInstance: IPublicClientApplication = new PublicClientApplication({
  auth: {
    clientId: "",
    authority: "",
    redirectUri: window.location.origin
  },
  cache: {
    cacheLocation: 'localStorage',
    storeAuthStateInCookie: false
  }
});
@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent

  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),

    HttpClientModule,
    MsalModule.forRoot(msalInstance, {
      interactionType: 'redirect',
      authRequest: {
        scopes: ['openid', 'profile', 'api://YOUR-API-IDENTIFIER/.default']
      }
    }, {
      protectedResourceMap: new Map([
        ['https://graph.microsoft.com/v1.0/', ['user.read']]
      ])
    })
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
