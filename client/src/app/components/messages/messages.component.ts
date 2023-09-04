import { Component, OnInit } from '@angular/core';
import { Pagination } from 'src/app/_models/pagination';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit{
  messages?: Message[];
  pagination?: Pagination;
  container = "Unread";
  pageNumber = 1;
  pageSize = 5;
  loading = false;
  
  constructor(private mesageService: MessageService){}

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(){
    this.loading = true;
    this.mesageService.getMessages(this.container, this.pageNumber, this.pageSize).subscribe({
      next: response => {
        this.messages = response.result;
        this.pagination = response.pagination;
        this.loading = false;
      }
    });
  }

  deleteMessage(id: number){
    this.mesageService.deleteMessage(id).subscribe({
      next: () => this.messages?.splice(this.messages.findIndex(m => m.id == id), 1)
    });
  }

  pageChanged(event: any){
    if(this.pageNumber === event.page) return;
    
    this.pageNumber = event.page;
    this.loadMessages();
  }

  hasMessages(){
    if(this.messages){
      return this.messages.length > 0;
    }
    return false;
  }

}
