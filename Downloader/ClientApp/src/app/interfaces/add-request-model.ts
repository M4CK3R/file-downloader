export interface AddRequestModel {
    Url: string;
    Name: string;
    Retry: boolean;
    MaxTries?: number;
}