import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit{
  @Input() usersFromHomeComponent: any;
  @Input() flag: string = "";
  model: any = { }
  constructor(){  } 

  ngOnInit(): void {
    console.log(this.flag);
  }

  register(){
    console.log(this.model);
  }

  cancel(){
    console.log("Cancelled");
  }

}
