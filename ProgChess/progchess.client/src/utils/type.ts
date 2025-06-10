
export type UnitTest = {
  id: number;
  code: string;
  isActive: boolean;
};

export type Exercise = {
  id: number;
  situation: string;
  baseCode: string;
  unitTests: UnitTest[];
  studentCodes: string[]
};

export type TestResult = {
  $type: string,
  testName: string,
  success: boolean,
  expected?: string,
  actual?: string
}

export type Score = {
  id: number,
  permanentCode: string,
  exercise: Exercise,
  scoreValue: number,
  answer: string
}

export type Error = {
  status?: number,
  message?: string 
}

export type TabType = {
  id: string,
  name: string
  isActive: boolean,
  component: React.ReactElement
}