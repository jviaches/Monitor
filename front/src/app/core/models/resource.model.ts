import { Periodicy } from '../enums/periodicy';

export interface IResource {
  id: number;
  url: string;
  userId: string;
  monitorActivationType: Periodicy;
  isMonitorActivated: boolean;
  monitorActivationDate: Date;
  history: IResourceHistory[];
  status: string;
}

export interface IResourceHistory {
  id: number;
  resourceId: string;
  monitorTypeId: string;
  requestDate: Date;
  responseDate: Date;
  result: string;
}
