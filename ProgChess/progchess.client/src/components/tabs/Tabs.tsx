import { useEffect, useState } from "react";
import type { TabType } from "../../utils/type";
import { Tab } from "./Tab";
import { useBadge } from "../../providers/ShowBadgeProvider";

interface TabsProps {
  tabs: TabType[];
}

export function Tabs({ tabs }: TabsProps) {
  const { badgeTabs, setBadgeTabs } = useBadge();

  const removeBadge = (tabKey: number) => {
    setBadgeTabs((prev) => ({
      ...prev,
      [tabKey]: false,
    }));
  };

  const findActiveTab = (a: TabType[]) => {
    return a.reduce((accumulator, currentValue, i) => {
      if (currentValue.isActive) {
        return i;
      }

      return accumulator;
    }, 0);
  };

  const [activeTab, setActiveTab] = useState(findActiveTab(tabs));

  useEffect(() => {
    if (tabs[activeTab].id == activeTab && badgeTabs[`${activeTab}`]) {
      removeBadge(activeTab);
    }
  }, [badgeTabs]);

  return (
    <>
      <div className="flex space-x-2 mt-2 overflow-x-auto">
        {tabs.map((item, i) => {
          return (
            <>
              <Tab
                key={`tab-{i}`}
                currentTab={i}
                activeTab={activeTab}
                setActiveTab={setActiveTab}
                showBadge={badgeTabs[`${item.id}`]}
                resetBadge={() => removeBadge(item.id)}
              >
                <div className="truncate overflow-auto whitespace-nowrap w-20 md:w-full md:whitespace-normal md:overflow-visible md:truncate-0">
                  {item.name}
                </div>
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
