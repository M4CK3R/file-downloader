import { Component, OnInit } from '@angular/core';
import { RequestService } from '../services/request.service';
import { RequestModel } from '../interfaces/request-model';
import { DownloadTaskService } from '../services/download-task.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  items: RequestModel[] = [];

  constructor(private requestService: RequestService, private downloadTaskService: DownloadTaskService) { }

  ngOnInit(): void {
    this.downloadTaskService.createEventSource().subscribe((data) => {
      const index = this.items.findIndex((item) => item.id == data.id);
      if (index == -1) {
        this.items.push(data);
        return;
      }
      // this.items[index] = data;
      this.items[index].progress = data.progress;
      this.items[index].isDone = data.isDone;
    });
    this.requestService.getAll().subscribe((data) => {
      this.items = data;
    });
  }

  inProgressCount() {
    return this.items.filter((item) => !item.isDone).length;
  }

  totalSpeed() {
    return this.items.reduce((acc, item) => acc + (item.isDone ? 0 : item.progress?.megaBytesPerSecond ?? 0), 0);
  }

  cpy(link: string) {
    navigator.clipboard.writeText(link);
  }

  remove(id: string) {
    this.requestService.delete(id).subscribe((data) => {
      const index = this.items.findIndex((item) => item.id == data.id);
      if (index == -1) return;
      this.items.splice(index, 1);
    });
  }
}
