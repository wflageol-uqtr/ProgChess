import { PanelRightClose } from "lucide-react";
import type { UnitTest } from "../../utils/type";
import TestTabs from "../tabs/TestTabs";

interface TestCaseCardProps {
  unitTests: UnitTest[];
  children: any;
}

export default function TestCaseCard({
  children,
  unitTests,
}: TestCaseCardProps) {
  return (
    <div className="bg-zinc-800 rounded-2xl h-full flex flex-col overflow-hidden">
      <div className="flex justify-between w-full p-2 items-center bg-zinc-700 rounded-t-2xl">
        <div className="flex gap-2 items-center">
          <PanelRightClose className="text-green-500" />
          <h4 className="font-semibold text-xl">Résultats des tests</h4>
        </div>
      </div>
      <TestTabs tabs={unitTests} />
      {children}
    </div>
  );
}
