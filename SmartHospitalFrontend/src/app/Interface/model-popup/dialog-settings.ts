export interface DialogSettings {
    Type:string,
    Icon:string,
    Title:string,
    DialogHeader:string,
    Message:string,
    IconColor:string,
    Button:Button,
}
export interface Button{
    Name:string,
    Action:Function,
    isDefaultVisible:boolean,
    isCancelVisible:boolean,
    }
