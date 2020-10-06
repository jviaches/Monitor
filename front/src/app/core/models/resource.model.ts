
export interface IResource {
  id: string;
  url: string;
  userId: string;
  monitoringActivated: boolean;
  monitorItem: IMonitorItem;
  communicationChanel: ICommunicationChanel;
}

export interface IResourceHistory {
  id: number;
  resourceId: string;
  scanDate: Date;
  result: string;
}

export interface IMonitorItem {
  id: number;
  resourceId: string;
  period: number;
  isActive: boolean;
  activationDate: Date;
  result: string;
  history: IResourceHistory[];
}

export interface ICommunicationChanel {
  id: number;
  resourceId: string;
  notifyByEmail: boolean;
  notifyBySlack: boolean;
  slackChanel: string;
}
