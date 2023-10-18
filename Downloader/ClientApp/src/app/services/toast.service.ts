import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Observer } from 'rxjs';
import { Toast } from '../interfaces/toast';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private toasts: BehaviorSubject<Toast | null> = new BehaviorSubject<Toast | null>(null);
  constructor() { }

  getToasts(): Observable<Toast | null> {
    return this.toasts.asObservable();
  }

  addToast(toast: Toast) {
    this.toasts.next(toast);
  }

  removeToast() {
    this.toasts.next(null);
  }
}
