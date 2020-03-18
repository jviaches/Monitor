import { Periodicy } from '../enums/periodicy';

export interface IResource {
  id: number;
  url: string;
  userId: string;
  monitorActivationType: Periodicy;
  isMonitorActivated: boolean;
  monitorActivationDate: Date;
}
