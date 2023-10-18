import { TestBed } from '@angular/core/testing';

import { DownloadTaskService } from './download-task.service';

describe('DownloadTaskService', () => {
  let service: DownloadTaskService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DownloadTaskService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
