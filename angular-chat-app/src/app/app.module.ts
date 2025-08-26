import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { ChatLayoutModule } from './chat-layout/chat-layout.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { LoginModule } from './auth/login/login.module';
import { RouterModule } from '@angular/router';
import { SignupModule } from './auth/signup/signup.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
      BrowserModule,
      ChatLayoutModule,
      BrowserAnimationsModule,
      AppRoutingModule,
      LoginModule,
      RouterModule,
      SignupModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
