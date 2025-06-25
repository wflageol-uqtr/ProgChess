import { useState } from "react";
import type { TabType } from "../../utils/type";
import { Tab } from "./Tab";

interface TabsProps {
  tabs: TabType[];
}

export function Tabs({ tabs }: TabsProps) {
  const findActiveTab = (a: TabType[]) => {
    return a.reduce((accumulator, currentValue, i) => {
      if (currentValue.isActive) {
        return i;
      }

      return accumulator;
    }, 0);
  };

  const [activeTab, setActiveTab] = useState(findActiveTab(tabs));
  return (
    <>
      <div className="flex space-x-2 mt-2">
        {tabs.map((item, i) => {
          return (
            <>
              <Tab
                key={`tab-{i}`}
                currentTab={i}
                activeTab={activeTab}
                setActiveTab={setActiveTab}
              >
                {item.name}
              </Tab>
            </>
          );
        })}
      </div>
      <div className="p-5">
        {tabs.map((item, i) => {
          return (
            <div className={` ${i === activeTab ? "visible" : "hidden"}`}>
              {item.component}
            </div>
          );
        })}
      </div>
    </>
  );
}
