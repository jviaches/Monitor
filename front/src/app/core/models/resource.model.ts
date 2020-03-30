import { Periodicy } from '../enums/periodicy';

export interface IResource {
  id: number;
  url: string;
  userId: string;
  monitorPeriod: Periodicy;
  isMonitorActivated: number;
  monitorActivationDate: Date;
  history: IResourceHistory[];
  status: string;
}

export interface IResourceHistory {
  id: number;
  resourceId: string;
  requestDate: Date;
  result: string;
}
