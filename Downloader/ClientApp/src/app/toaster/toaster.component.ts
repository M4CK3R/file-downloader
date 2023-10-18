import { Component, OnInit } from '@angular/core';
import { ToastService } from '../services/toast.service';
import { Toast, ToastType } from '../interfaces/toast';

@Component({
  selector: 'app-toaster',
  templateUrl: './toaster.component.html',
  styleUrls: ['./toaster.component.scss']
})
export class ToasterComponent implements OnInit {
  toasts: Toast[] = [];

  constructor(private toastService: ToastService) {

  }
  ngOnInit(): void {
    this.toastService.getToasts().subscribe((toast) => {
      if (toast) {
        this.spawnToast(toast);
      }
    });
  }

  spawnToast(toast: Toast) {
    this.toasts.push(toast);
  }

  deleteToast(toast: Toast) {
    return () =>{
      this.toasts = this.toasts.filter(t => t != toast);
    }
  }
}
