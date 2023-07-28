import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit{
  
  model: any = {};
  loggedIn = false;
  username = "";

  constructor(private accountService: AccountService){}

  ngOnInit(): void { 
    this.getCurrentUser();
  }

  getCurrentUser(){
    this.accountService.currentUser$.subscribe({
      next: user => { 
        this.loggedIn = !!user;              
      },
      error: error => console.log(error)
    })
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next: Response => {
        this.loggedIn = true;
      },
      error: error => {
        this.loggedIn = false;
      }
    })
  }

  logout(){ 
    this.loggedIn = false;
    this.accountService.logout();
  }

}
