  export interface RequestModel {
    id: string;
    name: string;
    url: string;
    isDone: boolean;
    progress?: ProgressInfoModel;
  }
  
  export interface ProgressInfoModel {
    downloadStartedAt: Date;
    bytesDownloaded: number;
    bytesTotal: number;
    megaBytesDownloaded: number;
    megaBytesTotal: number;
    percentage: number;
    bytesPerSecond: number;
    megaBytesPerSecond: number;
  }