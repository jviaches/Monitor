
export interface IResource {
  id: number;
  url: string;
  userId: string;
  monitorPeriod: number;
  isMonitorActivated: boolean;
  monitorActivationDate: Date;
  history: IResourceHistory[];
  lastStatus: string;
}

export interface IResourceHistory {
  id: number;
  resourceId: string;
  requestDate: Date;
  result: string;
}
