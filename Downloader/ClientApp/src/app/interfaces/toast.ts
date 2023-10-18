export interface Toast {
    title: string;
    message: string;
    type: ToastType;
}

export enum ToastType {
    Success = "Success",
    Error = "Danger",
    Info = "Info",
    Warning = "Warning"
}
