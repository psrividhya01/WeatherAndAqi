export interface Alert {
  alertId: string;
  city: string;
  alertType: string;
  severity: 'low' | 'medium' | 'high' | 'critical';
  message: string;
  createdAt: Date;
}

export interface ActiveAlert extends Alert {
  isDismissed?: boolean;
}
