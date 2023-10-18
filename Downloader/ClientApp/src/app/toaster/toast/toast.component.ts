import { Component, Input, OnInit } from '@angular/core';
import { ToastType } from 'src/app/interfaces/toast';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.scss']
})
export class ToastComponent implements OnInit {
  @Input() title: string = '';
  @Input() message: string = '';
  @Input() type: ToastType = ToastType.Success;
  @Input() isDismissed: boolean = false;
  @Input() OnDismiss: Function = () => { };
  @Input() lifeExpectancy: number = 100;
  delayMs: number = 5000;
  timeout: NodeJS.Timeout | undefined = undefined;
  dismissible: boolean = true;

  constructor() {
    document.documentElement.style.setProperty('--toast-delay', `${this.delayMs}ms`);
  }

  ngOnInit(): void {
    this.setTimer();
  }

  blockDismiss() {
    this.dismissible = false;
    this.resetTimer();
  }

  unblockDismiss() {
    this.dismissible = true;
    this.resetTimer();
  }

  resetTimer() {
    this.isDismissed = false;
    if (this.timeout)
      clearTimeout(this.timeout);
    this.setTimer();
  }

  setTimer() {
    if (!this.dismissible) return;
    this.timeout = setTimeout(() => this.dismissToast(false), this.lifeExpectancy);
  }

  getToastClass() {
    const c = `bg-${this.type.toString().toLowerCase()}`;
    return c;
  }

  dismissToast(noDelay: boolean = true) {
    this.isDismissed = true;
    this.timeout = setTimeout(() => this.OnDismiss(), noDelay ? 0 : this.delayMs);
  }
}
