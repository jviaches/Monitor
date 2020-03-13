
export interface IResource {
  id: number;
  url: string;
  userId: string;
  monitorPeriod: string;
  isMonitorActive: boolean;
  monitorLastActivationDate: Date;
}
